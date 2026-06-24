export interface CreateLoanPayload {
  amount: number;
  currentBalance: number;
  applicantName: string;
}

export interface AuthRequest {
  username: string;
  password: string;
}

export interface AuthResponse {
  token: string;
  expiresInMinutes: number;
}
