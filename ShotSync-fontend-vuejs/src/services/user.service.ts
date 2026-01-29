import api from './api';
import type { UserResponseDto, UpdateUserDto } from '../types/schema';

class UserService {
    async getUsers(): Promise<UserResponseDto[]> {
        const response = await api.get<UserResponseDto[]>('/api/users');
        return response.data;
    }

    async getUser(id: number): Promise<UserResponseDto> {
        // Check if endpoint is /users/{id} or /user/{id}. Swagger usually uses plural.
        const response = await api.get<UserResponseDto>(`/api/users/${id}`);
        return response.data;
    }

    async updateUser(id: number, data: UpdateUserDto): Promise<UserResponseDto> {
        const response = await api.put<UserResponseDto>(`/api/users/${id}`, data);
        return response.data;
    }

    async deleteUser(id: number): Promise<void> {
        await api.delete(`/api/users/${id}`);
    }
}

export default new UserService();
