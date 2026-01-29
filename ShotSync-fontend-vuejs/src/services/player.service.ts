import api from './api';
import type { Player, PlayerHistory, RegisterPlayerDto } from '../types/schema';

class PlayerService {
    async getPlayers(): Promise<Player[]> {
        const response = await api.get<Player[]>('/api/players');
        return response.data;
    }

    async getPlayer(id: number): Promise<Player> {
        const response = await api.get<Player>(`/api/players/${id}`);
        return response.data;
    }

    async createPlayer(data: Player): Promise<Player> {
        const response = await api.post<Player>('/api/players', data);
        return response.data;
    }

    async updatePlayer(id: number, data: Player): Promise<Player> {
        const response = await api.put<Player>(`/api/players/${id}`, data);
        return response.data;
    }

    async deletePlayer(id: number): Promise<void> {
        await api.delete(`/api/players/${id}`);
    }

    async registerPlayerToEvent(data: RegisterPlayerDto): Promise<void> {
        await api.post('/api/players/register', data);
    }

    async getPlayerHistory(playerId: number): Promise<PlayerHistory[]> {
        const response = await api.get<PlayerHistory[]>(`/api/players/${playerId}/history`);
        return response.data;
    }

    async getPlayersByEvent(eventId: number): Promise<Player[]> {
        const response = await api.get<Player[]>(`/api/players/by-event/${eventId}`);
        return response.data;
    }
}

export default new PlayerService();
