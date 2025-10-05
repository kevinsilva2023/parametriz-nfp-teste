import { jwtDecode, JwtPayload } from 'jwt-decode';
import moment from 'moment';

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
        localStorage.setItem('autoNFP.refreshToken', refreshToken);
    }

    public static salvarUsuario(user: string) {
        localStorage.setItem('autoNFP.userToken', JSON.stringify(user));
    }

    public static limparDadosLocaisUsuario() {
        localStorage.removeItem('autoNFP.accessToken');
        localStorage.removeItem('autoNFP.refreshToken');
        localStorage.removeItem('autoNFP.userToken');
    }

    public static obterUsuario() {
        return JSON.parse(localStorage.getItem('autoNFP.userToken')?.toString() ?? '');
    }

    public static obterAccessToken(): string {
        return localStorage.getItem('autoNFP.accessToken')?.toString() ?? '';
    }

    private static obterAccessTokenExpiration() {
        const accessToken = this.obterAccessToken();

        if (!accessToken)
            return null;

        const payload = <JwtPayload>jwtDecode(accessToken);

        if (!payload)
            return null;

        const expiresAt = moment.unix(payload?.exp ?? 0);

        return moment(expiresAt.valueOf());
    }

    public static accessTokenEstaExpirado(): boolean {
        const token = this.obterAccessToken();

        if (!token)
            return false;

        return moment().isAfter(this.obterAccessTokenExpiration());
    }

    public static obterRefreshToken(): string {
        return localStorage.getItem('autoNFP.refreshToken')?.toString() ?? '';
    }

    private static obterRefreshTokenExpiration() {
        const refreshToken = this.obterRefreshToken();

        if (!refreshToken)
            return null;

        const payload = <JwtPayload>jwtDecode(refreshToken);

        if (!payload)
            return null;

        const expiresAt = moment.unix(payload?.exp ?? 0);

        return moment(expiresAt.valueOf());
    }

    public static refreshTokenEstaExpirado(): boolean {
        const refreshToken = this.obterRefreshToken();

        if (!refreshToken)
            return false;

        return moment().isAfter(this.obterRefreshTokenExpiration());
    }

    public static obterInstituicaoId(): string {
        const userToken = this.obterUsuario();

        return userToken?.instituicaoId ?? '';
    }
}
