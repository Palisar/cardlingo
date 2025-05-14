import axios from 'axios';
import { 
  LoginRequest, 
  RegisterRequest, 
  AuthResponse, 
  User, 
  Deck, 
  Card 
} from '../types/models';

// Create an axios instance with default config
const api = axios.create({
  baseURL: 'https://localhost:7123/api',
  headers: {
    'Content-Type': 'application/json'
  }
});

// Add request interceptor to include the auth token in all requests
api.interceptors.request.use(
  (config) => {
    const token = localStorage.getItem('token');
    if (token) {
      config.headers.Authorization = `Bearer ${token}`;
    }
    return config;
  },
  (error) => Promise.reject(error)
);

// Authentication Service
export const authService = {
  login: async (credentials: LoginRequest): Promise<AuthResponse> => {
    const response = await api.post<AuthResponse>('/auth/login', credentials);
    
    // Save the token to localStorage
    localStorage.setItem('token', response.data.token);
    
    return response.data;
  },
  
  register: async (userData: RegisterRequest): Promise<string> => {
    const response = await api.post<string>('/auth/register', userData);
    return response.data;
  },
  
  logout: (): void => {
    localStorage.removeItem('token');
  },
  
  isAuthenticated: (): boolean => {
    return !!localStorage.getItem('token');
  }
};

// Users Service
export const userService = {
  getCurrentUser: async (): Promise<User> => {
    const response = await api.get<User>('/users/current');
    return response.data;
  },
  
  getUserById: async (id: number): Promise<User> => {
    const response = await api.get<User>(`/users/${id}`);
    return response.data;
  }
};

// Decks Service
export const deckService = {
  getDecks: async (): Promise<Deck[]> => {
    const response = await api.get<Deck[]>('/decks');
    return response.data;
  },
  
  getDeckById: async (id: number): Promise<Deck> => {
    const response = await api.get<Deck>(`/decks/${id}`);
    return response.data;
  },
  
  createDeck: async (deck: Omit<Deck, 'id'>): Promise<Deck> => {
    const response = await api.post<Deck>('/decks', deck);
    return response.data;
  },
  
  updateDeck: async (id: number, deck: Deck): Promise<Deck> => {
    const response = await api.put<Deck>(`/decks/${id}`, deck);
    return response.data;
  },
  
  deleteDeck: async (id: number): Promise<void> => {
    await api.delete(`/decks/${id}`);
  }
};

// Cards Service
export const cardService = {
  getCardsByDeckId: async (deckId: number): Promise<Card[]> => {
    const response = await api.get<Card[]>(`/decks/${deckId}/cards`);
    return response.data;
  },
  
  getCardById: async (id: number): Promise<Card> => {
    const response = await api.get<Card>(`/cards/${id}`);
    return response.data;
  },
  
  createCard: async (card: Omit<Card, 'id'>): Promise<Card> => {
    const response = await api.post<Card>('/cards', card);
    return response.data;
  },
  
  updateCard: async (id: number, card: Card): Promise<Card> => {
    const response = await api.put<Card>(`/cards/${id}`, card);
    return response.data;
  },
  
  deleteCard: async (id: number): Promise<void> => {
    await api.delete(`/cards/${id}`);
  }
};
