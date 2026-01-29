/**
 * Authentication guard for route navigation.
 */
import type { NavigationGuardNext, RouteLocationNormalized } from 'vue-router';
import { UserRole, parseUserRole } from 'src/types/role.enum';
import ROUTE_PATHS from '../route-paths';

/**
 * Check if user is authenticated.
 */
export function isAuthenticated(): boolean {
  return !!(localStorage.getItem('Token') || sessionStorage.getItem('Token'));
}

/**
 * Get current user role.
 */
export function getCurrentUserRole(): UserRole | null {
  const roleString = localStorage.getItem('role') || sessionStorage.getItem('role');
  if (!roleString) return null;
  return parseUserRole(roleString);
}

/**
 * Get dashboard route based on user role.
 */
export function getDashboardRoute(role: UserRole): string {
  switch (role) {
    case UserRole.ADMIN:
      return ROUTE_PATHS.ADMIN.APP_STATUS;
    case UserRole.HOST:
      return ROUTE_PATHS.HOST.PROFILE;
    case UserRole.PLAYER:
      return ROUTE_PATHS.PLAYER.NEWS;
    default:
      return ROUTE_PATHS.PLAYER.ROOT;
  }
}

import { Dialog, Notify } from 'quasar';

/**
 * Main authentication guard.
 */
export function authGuard(
  to: RouteLocationNormalized,
  from: RouteLocationNormalized,
  next: NavigationGuardNext
): void {
  const token = localStorage.getItem('Token') || sessionStorage.getItem('Token');
  const userRole = getCurrentUserRole();

  // Handle root path
  console.log(`[AuthGuard] Navigating to: ${to.path}. Token: ${!!token}, Role: ${userRole}`);

  if (to.path === '/') {
    // If logged in, redirect to dashboard
    if (token && userRole) {
      console.log(`[AuthGuard] Root redirect -> Dashboard: ${getDashboardRoute(userRole)}`);
      return next({ path: getDashboardRoute(userRole) });
    }
    // If not logged in, just let them go to the default child (News) which is public
    console.log(`[AuthGuard] Root redirect -> Public default`);
    return next();
  }

  // Handle Login path for logged-in users
  if (to.path === ROUTE_PATHS.AUTH.LOGIN && token) {
    Dialog.create({
      title: 'Confirm Logout',
      message: 'You are already logged in. Do you want to logout?',
      cancel: {
        label: 'Cancel',
        color: 'primary',
        flat: true
      },
      persistent: true,
      ok: {
        label: 'Logout',
        color: 'negative',
        flat: true
      }
    }).onOk(() => {
      // User confirmed logout
      localStorage.removeItem('Token');
      sessionStorage.removeItem('Token');
      localStorage.removeItem('role');
      sessionStorage.removeItem('role');
      localStorage.removeItem('userName');
      sessionStorage.removeItem('userName');

      Notify.create({
        color: 'positive',
        message: 'Logged out successfully',
        icon: 'check',
        position: 'top',
      });
      next();
    }).onCancel(() => {
      // User canceled, redirect back to dashboard or stay on current page (but we are navigating TO login, so better redirect to dashboard)
      if (userRole) {
        next({ path: getDashboardRoute(userRole) });
      } else {
        // Fallback if no role but has token (weird state), force logout
        localStorage.removeItem('Token');
        sessionStorage.removeItem('Token');
        next();
      }
    });
    return;
  }

  // Check strict role-based access
  const allowedRoles = to.meta.allowedRoles as UserRole[] | undefined;
  const allowGuest = to.meta.allowGuest as boolean | undefined;

  if (allowedRoles) {
    if (!token) {
      if (allowGuest) {
        console.log(`[AuthGuard] Guest allowed on ${to.path}`);
        return next(); // Guest is specifically allowed
      }
      console.log(`[AuthGuard] Login required for ${to.path}`);
      return next({ path: ROUTE_PATHS.AUTH.LOGIN }); // Otherwise require login
    }

    // Checking logged in user role
    const hasRole = userRole && allowedRoles.includes(userRole);
    console.log(`[AuthGuard] Checking Role: User=${userRole}, Allowed=${JSON.stringify(allowedRoles)}. Match=${hasRole}`);

    if (userRole && !allowedRoles.includes(userRole)) {
      console.warn(`[AuthGuard] Access denied. Role ${userRole} not in ${JSON.stringify(allowedRoles)}`);
      return next({ path: ROUTE_PATHS.ERROR.UNAUTHORIZED });
    }

    if (!userRole) {
      console.warn(`[AuthGuard] Token without role -> Login`);
      return next({ path: ROUTE_PATHS.AUTH.LOGIN });
    }
  }

  next();
}
