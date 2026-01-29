export interface Player {
  name: string;
  nation: string;
  portrait?: string;
  point: string;
}

export interface Event {
  id: number;
  name: string;
  venue: string;
  location: string;
  date: string;
  isHappen: boolean;
}

export interface Match{
  id: number;
  eventName: string;
  table: string;
  firstPlayerName: string;
  firstPlayerPoint: number;
  secondPlayerName: string;
  secondPlayerPoint: number;
  isFinish: boolean;
  isStart: boolean;
  stage: number;
}
