import api from './api';
import type { Match } from '../types/schema';

class MatchService {
    async getMatchesByEvent(eventId: number): Promise<Match[]> {
        const response = await api.get<Match[]>(`/api/matches/by-event/${eventId}`);
        return response.data;
    }

    async updateScore(matchId: number, firstScore: number, secondScore: number): Promise<void> {
        await api.put(`/api/matches/${matchId}/score`, {
            firstPlayerScore: firstScore,
            secondPlayerScore: secondScore
        });
    }

    async finishMatch(matchId: number): Promise<void> {
        await api.post(`/api/matches/${matchId}/finish`);
    }
}

export default new MatchService();
