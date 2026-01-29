import api from './api';
import type { LoginRequestDto, RegisterUserDto, UserLoginDto } from '../types/schema';

class AuthService {
  async login(data: LoginRequestDto): Promise<UserLoginDto> {
    const response = await api.post<UserLoginDto>('/api/users/login', data);
    return response.data;
  }

  async register(data: RegisterUserDto): Promise<void> {
    await api.post('/api/users/register', data);
  }

  // Assuming logout might just be client side, but if there was an endpoint:
  // async logout(): Promise<void> {
  //   await api.post('/logout');
  // }
}

export default new AuthService();
