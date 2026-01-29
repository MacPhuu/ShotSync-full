/**
 * User role enumeration matching backend.
 *
 * IMPORTANT: These values must match the backend UserRole enum.
 */

export enum UserRole {
  ADMIN = 1,
  HOST = 2,
  PLAYER = 3,
}

/**
 * Helper function to get role display name.
 */
export function getRoleDisplayName(role: UserRole): string {
  const displayNames: Record<UserRole, string> = {
    [UserRole.ADMIN]: 'Administrator',
    [UserRole.HOST]: 'Host',
    [UserRole.PLAYER]: 'Player',
  };
  return displayNames[role];
}

/**
 * Helper function to parse role from string or number.
 */
export function parseUserRole(roleInput: string | number | null | undefined): UserRole | null {
  if (roleInput === null || roleInput === undefined) return null;

  // If it's already a number and a valid enum value
  if (typeof roleInput === 'number' && Object.values(UserRole).includes(roleInput as UserRole)) {
    return roleInput as UserRole;
  }

  const roleString = String(roleInput).toLowerCase();

  // Check for numeric string "1", "2"
  const roleNumber = Number(roleString);
  if (!isNaN(roleNumber) && Object.values(UserRole).includes(roleNumber as UserRole)) {
    return roleNumber as UserRole;
  }

  // Check for string names "admin", "host"
  if (roleString === 'admin') return UserRole.ADMIN;
  if (roleString === 'host') return UserRole.HOST;
  if (roleString === 'player' || roleString === 'user') return UserRole.PLAYER;

  return null;
}

export default UserRole;
