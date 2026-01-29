import type { RouteRecordRaw } from 'vue-router';
import { UserRole } from 'src/types/role.enum';
import ROUTE_PATHS from './route-paths';

const routes: RouteRecordRaw[] = [
  // Root redirect - handled by auth guard
  {
    path: '/',
    redirect: () => {
      // This will be intercepted by auth guard
      return ROUTE_PATHS.PLAYER.NEWS;
    },
  },

  // Authentication routes (no auth required)
  {
    path: ROUTE_PATHS.AUTH.LOGIN,
    name: 'Login',
    component: () => import('pages/Authentication/LoginPage.vue'),
  },
  {
    path: ROUTE_PATHS.AUTH.SIGN_UP,
    name: 'SignUp',
    component: () => import('pages/Authentication/SignUpPage.vue'),
  },

  // Player/Public routes
  {
    path: '/player',
    component: () => import('layouts/PlayerLayout.vue'),
    // Strict Separation:
    // - Allowed for: Players
    // - Allowed for: Guests
    // - BLOCKED for: Admin, Host
    meta: {
      allowedRoles: [UserRole.PLAYER],
      // allowGuest removed - STRICT AUTHENTICATION REQUIRED
    },
    children: [
      {
        path: 'news', // Relative path, becomes /news
        name: 'News',
        component: () => import('pages/Players/NewsPage.vue'),
      },
      {
        path: 'players',
        name: 'Players',
        component: () => import('pages/Players/PlayersPage.vue'),
      },
      {
        path: 'rankings',
        name: 'Rankings',
        component: () => import('pages/Players/RankingsPage.vue'),
      },
      {
        path: 'live-scores',
        name: 'LiveScores',
        component: () => import('pages/Players/LiveScoresPage.vue'),
      },
      {
        path: 'events',
        name: 'Events',
        component: () => import('pages/Players/EventsPage.vue'),
      },
      {
        path: 'events/:eventName',
        component: () => import('layouts/EventLayout.vue'),
        children: [
          {
            path: '',
            name: 'EventDetail',
            component: () => import('pages/Events/EventDetailPage.vue'),
          },
          {
            path: 'rankings',
            name: 'EventRankings',
            component: () => import('pages/Events/EventRankingsPage.vue'),
          },
          {
            path: 'live-score',
            name: 'EventLiveScore',
            component: () => import('pages/Events/EventLiveScoresPage.vue'),
          },
          {
            path: 'brackets',
            name: 'EventBrackets',
            component: () => import('pages/Events/EventBranchesPage.vue'),
          },
        ],
      },
      {
        path: 'players/:playerName',
        name: 'PlayerDetail',
        component: () => import('pages/Players/PlayerDetailPage.vue'),
      },
    ],
  },

  // Host routes
  {
    path: ROUTE_PATHS.HOST.ROOT,
    component: () => import('layouts/HostLayout.vue'),
    // Strict Separation: Host Only (Removed Admin)
    meta: {
      allowedRoles: [UserRole.HOST]
    },
    children: [
      {
        path: 'profile', // /host/profile
        name: 'HostProfile',
        component: () => import('pages/Host/HostMainPage.vue'),
      },
      {
        path: 'events',
        name: 'HostEvents',
        component: () => import('pages/Host/HostEventsPage.vue'),
      },
      {
        path: 'create-event',
        name: 'HostCreateEvent',
        alias: 'create_event', // Support snake_case access
        component: () => import('pages/Host/HostCreateEventPage.vue'),
      },
      {
        path: 'events/:eventName',
        component: () => import('layouts/EventLayout.vue'),
        children: [
          {
            path: '',
            name: 'HostEventDetail',
            component: () => import('pages/Events/EventDetailPage.vue'),
          },
          {
            path: 'rankings',
            name: 'HostEventRankings',
            component: () => import('pages/Events/EventRankingsPage.vue'),
          },
          {
            path: 'live-score',
            name: 'HostEventLiveScore',
            component: () => import('pages/Events/EventLiveScoresPage.vue'),
          },
          {
            path: 'brackets',
            name: 'HostEventBrackets',
            component: () => import('pages/Events/EventBranchesPage.vue'),
          },
        ],
      },
    ],
  },

  // Admin routes
  {
    path: ROUTE_PATHS.ADMIN.ROOT,
    component: () => import('layouts/AdminLayout.vue'),
    meta: {
      allowedRoles: [UserRole.ADMIN]
    },
    children: [
      {
        path: 'app-status',
        name: 'AdminAppStatus',
        component: () => import('pages/Admin/AdminMainPage.vue'),
      },
      {
        path: 'hosts',
        name: 'AdminHosts',
        component: () => import('pages/Admin/AdminViewHostsPage.vue'),
      },
      {
        path: 'players',
        name: 'AdminPlayers',
        component: () => import('pages/Admin/AdminViewPlayersPage.vue'),
      },
    ],
  },

  // Error routes
  {
    path: ROUTE_PATHS.ERROR.UNAUTHORIZED,
    name: 'Unauthorized',
    component: () => import('pages/ErrorUnauthorized.vue'),
  },
  {
    path: '/:catchAll(.*)*',
    name: 'NotFound',
    component: () => import('pages/ErrorNotFound.vue'),
  },
];

export default routes;
