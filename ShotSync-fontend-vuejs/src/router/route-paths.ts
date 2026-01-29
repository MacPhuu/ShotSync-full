/**
 * Centralized route path constants.
 *
 * Use these constants instead of hardcoded strings for type safety
 * and easier refactoring.
 */

export const ROUTE_PATHS = {
  // Authentication routes
  AUTH: {
    LOGIN: '/login',
    SIGN_UP: '/sign-up',
  },

  // Admin routes
  ADMIN: {
    ROOT: '/admin',
    APP_STATUS: '/admin/app-status',
    HOSTS: '/admin/hosts',
    PLAYERS: '/admin/players',
  },

  // Host routes
  HOST: {
    ROOT: '/host',
    PROFILE: '/host/profile',
    EVENTS: '/host/events',
    CREATE_EVENT: '/host/create-event',
    EVENT_DETAIL: (eventName: string) => `/host/events/${eventName}`,
    EVENT_RANKINGS: (eventName: string) => `/host/events/${eventName}/rankings`,
    EVENT_LIVE_SCORE: (eventName: string) => `/host/events/${eventName}/live-score`,
    EVENT_BRACKETS: (eventName: string) => `/host/events/${eventName}/brackets`,
  },

  // Player/Public routes
  PLAYER: {
    ROOT: '/player',
    NEWS: '/player/news',
    EVENTS: '/player/events',
    PLAYERS: '/player/players',
    RANKINGS: '/player/rankings',
    LIVE_SCORES: '/player/live-scores',
    EVENT_DETAIL: (eventName: string) => `/player/events/${eventName}`,
    EVENT_RANKINGS: (eventName: string) => `/player/events/${eventName}/rankings`,
    EVENT_LIVE_SCORE: (eventName: string) => `/player/events/${eventName}/live-score`,
    EVENT_BRACKETS: (eventName: string) => `/player/events/${eventName}/brackets`,
    PLAYER_DETAIL: (playerName: string) => `/player/players/${playerName}`,
  },

  // Error routes
  ERROR: {
    NOT_FOUND: '/404',
    UNAUTHORIZED: '/401',
  },
} as const;

export default ROUTE_PATHS;
