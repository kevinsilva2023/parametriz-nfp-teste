import { HttpRequest } from "@angular/common/module.d-CnjH8Dlt";
import { BehaviorSubject, Subject } from "rxjs";
import { LocalStorageUtils } from "../utils/local-storage-utils";

export class JwtSupport {
    public static refreshTokenInProgress = false;
    public static refreshTokenSubject: Subject<any> = new BehaviorSubject<any>(null);

    public static injectAccessToken(request: HttpRequest<any>) {
        const accessToken = LocalStorageUtils.obterAccessToken();
        
        return request.clone({
            setHeaders: {
                Authorization: `Bearer ${accessToken}`
            }
        });
  }
}
