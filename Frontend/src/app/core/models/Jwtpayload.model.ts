export interface JwtPayload {
    unique_name: string;
    nameId: number;
    email: string;
    role: string;
    exp: number;
}