import { jwtDecode, JwtPayload } from 'jwt-decode';
import moment from 'moment';
import { JwtPayloadExtension } from '../models/jwt-payload-extension';

export class LocalStorageUtils {

    public static salvarDadosLocaisUsuario(response: any) {
        this.salvarAccessToken(response.accessToken);
        this.salvarRefreshToken(response.refreshToken);
        this.salvarUsuario(response.userToken);
    }

    public static salvarAccessToken(token: string) {
        localStorage.setItem('autoNFP.accessToken', token);
    }

    public static salvarRefreshToken(refreshToken: string) {
        localStorage.setItem('autoNFP.refreshToken', JSON.stringify(refreshToken));
    }

    public static salvarUsuario(user: string) {
        localStorage.setItem('autoNFP.user', JSON.stringify(user));
    }

    public static limparDadosLocaisUsuario() {
        localStorage.removeItem('autoNFP.accessToken');
        localStorage.removeItem('autoNFP.refreshToken');
        localStorage.removeItem('autoNFP.user');
    }

    public static obterAccessToken(): string {
        return localStorage.getItem('autoNFP.accessToken')?.toString() ?? '';
    }

    public static obterRefreshToken() {
        return JSON.parse(localStorage.getItem('autoNFP.refreshToken')?.toString() ?? '');
    }

    public static obterUsuario() {
        return JSON.parse(localStorage.getItem('autoNFP.user')?.toString() ?? '');
    }

    public static obterAccessTokenExpiration() {
        const accessToken = this.obterAccessToken();

        if (!accessToken)
            return null;

        const payload = <JwtPayload>jwtDecode(accessToken);

        if (!payload)
            return null;

        const expiresAt = moment.unix(payload?.exp ?? 0);

        return moment(expiresAt.valueOf());
    }

    public static obterRefreshTokenExpiration() {
        const refreshToken = this.obterRefreshToken();

        if (!refreshToken)
            return null;

        const payload = <JwtPayload>jwtDecode(refreshToken);

        if (!payload)
            return null;

        const expiresAt = moment.unix(payload?.exp ?? 0);

        return moment(expiresAt.valueOf());
    }

    public static obterInstituicaoId(): string {
        const accessToken = this.obterAccessToken();

        if (!accessToken)
            return '';

        const payload = <JwtPayloadExtension>jwtDecode(accessToken);

        if (!payload)
            return '';

        return payload?.instituicaoId ?? '';
    }
}
