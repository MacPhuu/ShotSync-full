export interface Event {
    id: number;
    name?: string | null;
    venue?: string | null;
    location?: string | null;
    date: string; // date-time
    numberOfPlayers: number;
    slogan?: string;
    format?: string;
    entryFee?: number;
    totalPrize?: number;
    description?: string;
    status: EventStatus;
    isDisplayed: boolean;
    isHappen: boolean;
    createdAt?: string | null; // date-time
    updateAt?: string | null; // date-time
}

export enum EventStatus {
    Status0 = 0,
    Status1 = 1,
    Status2 = 2,
    Status3 = 3,
    Status4 = 4,
}

export interface LoginRequestDto {
    email: string; // email
    password: string;
}

export interface Player {
    id?: number;
    name?: string | null;
    nation?: string | null;
    portrait?: string | null;
    point?: number | null;
    isActive: boolean;
    createdAt?: string | null; // date-time
    updateAt?: string | null; // date-time
}

export interface PlayerHistory {
    id?: number;
    playerId?: number;
    eventId?: number;
    matchId?: number;
    result?: string | null;
    opponentId?: number;
    matchDate?: string; // date-time
    player?: Player;
    event?: Event;
}

export interface PlayerHistoryDto {
    playerId?: number;
    matchId?: number;
    result?: string | null;
    opponentId?: number;
    eventId?: number;
}

export interface RegisterPlayerDto {
    playerId?: number;
    eventId?: number;
}

export interface RegisterUserDto {
    name: string;
    email: string; // email
    password: string; // minLength: 6
    nation?: string | null;
}

export interface UpdateScoreDto {
    firstPlayerScore?: number;
    secondPlayerScore?: number;
}

export interface UpdateUserDto {
    name: string;
    nation?: string | null;
    avatar?: string | null;
}

export interface Match {
    id: number;
    eventId: number;
    eventName?: string;
    roundName?: string;
    roundType: number;
    table?: string;
    tableNumber?: string;
    firstPlayerName?: string;
    firstPlayerId?: number;
    firstPlayerPoint: number;
    secondPlayerName?: string;
    secondPlayerId?: number;
    secondPlayerPoint: number;
    isFinish: boolean;
    isStart: boolean;
    nextMatchPosition: number;
}

export interface UserLoginDto {
    id?: number;
    userName: string;
    email?: string | null;
    role: string;
    token: string;
}

export interface UserResponseDto {
    id?: number;
    name?: string | null;
    email?: string | null;
    nation?: string | null;
    avatar?: string | null;
    role?: number;
    roleName?: string | null;
    isActive?: boolean;
    createdAt?: string; // date-time
    updateAt?: string | null; // date-time
}
