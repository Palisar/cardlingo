// Models for TypeScript
export interface User {
  id: number;
  username: string;
  email: string;
  created: string;
  lastLogin: string;
}

export interface Card {
  id: number;
  question: string;
  answer: string;
  notes?: string;
  created: string;
  lastModified: string;
  deckId: number;
}

export interface Deck {
  id: number;
  name: string;
  description: string;
  created: string;
  lastModified: string;
  userId: number;
  cards?: Card[];
}

export interface Pile {
  id: number;
  name: string;
  order: number;
  deckId: number;
}

export interface ReviewHistory {
  id: number;
  reviewDate: string;
  score: number;
  responseTime: string;
  cardId: number;
  userId: number;
}

export interface Goal {
  id: number;
  title: string;
  description: string;
  targetCount: number;
  startDate: string;
  endDate: string;
  isCompleted: boolean;
  userId: number;
}

// Authentication Interfaces
export interface LoginRequest {
  username: string;
  password: string;
}

export interface RegisterRequest {
  username: string;
  email: string;
  password: string;
  confirmPassword: string;
}

export interface AuthResponse {
  token: string;
}
