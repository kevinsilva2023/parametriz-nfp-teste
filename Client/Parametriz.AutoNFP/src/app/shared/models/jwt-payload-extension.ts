import { JwtPayload } from 'jwt-decode';

export interface JwtPayloadExtension extends JwtPayload {
    instituicaoId: string;
}
