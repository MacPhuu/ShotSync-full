import api from './api';
import type { Event, UpdateScoreDto } from '../types/schema';

class EventService {
    async getEvents(): Promise<Event[]> {
        const response = await api.get<Event[]>('/api/events');
        return response.data;
    }

    async getEvent(id: number): Promise<Event> {
        const response = await api.get<Event>(`/api/events/${id}`);
        return response.data;
    }

    async getEventsByHost(hostId: number): Promise<Event[]> {
        const response = await api.get<Event[]>(`/api/events/host/${hostId}`);
        return response.data;
    }

    async getEventsByName(name: string): Promise<Event[]> {
        const response = await api.get<Event[]>(`/api/events/search/${name}`);
        return response.data;
    }

    // Assuming these endpoints exist based on standard REST patterns, verify against Swagger if needed.
    // The provided swagger dump showed some schemas but not all paths, so I'll infer standard CRUD for now
    // and user can refine if endpoints are named differently.

    async createEvent(data: Partial<Event>): Promise<Event> {
        const response = await api.post<Event>('/api/events', data);
        return response.data;
    }

    async updateEvent(id: number, data: Partial<Event>): Promise<Event> {
        const response = await api.put<Event>(`/api/events/${id}`, data);
        return response.data;
    }

    async deleteEvent(id: number): Promise<void> {
        await api.delete(`/api/events/${id}`);
    }

    async updateScore(matchId: number, data: UpdateScoreDto): Promise<void> {
        // Endpoint inferred, might need adjustment based on strict swagger path for score updates
        await api.put(`/api/matches/${matchId}/score`, data);
    }

    async registerPlayerByEmail(eventId: number, email: string): Promise<void> {
        // The backend expects just the string in the body, but usually json.
        // Based on typical axios usage:
        await api.post(`/api/events/${eventId}/register-player`, JSON.stringify(email), {
            headers: { 'Content-Type': 'application/json' }
        });
    }

    async startTournament(eventId: number): Promise<void> {
        await api.post(`/api/events/${eventId}/start`);
    }

    async seedPlayers(eventId: number, count: number = 16): Promise<void> {
        await api.post(`/api/seed/event/${eventId}?count=${count}`);
    }

    async fillPlayers(eventId: number): Promise<void> {
        await api.post(`/api/seed/event/${eventId}/fill`);
    }

    async resetTournament(eventId: number): Promise<void> {
        await api.post(`/api/events/${eventId}/reset`);
    }
}

export default new EventService();
