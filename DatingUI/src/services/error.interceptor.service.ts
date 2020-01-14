import { Injectable } from '@angular/core';
import { HttpInterceptor, HttpErrorResponse, HTTP_INTERCEPTORS } from '@angular/common/http';
import { catchError } from 'rxjs/operators';
import { throwError } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class ErrorInterceptor implements HttpInterceptor {

  constructor() { }
  intercept(
    req: import("@angular/common/http").HttpRequest<any>,
    next: import("@angular/common/http").HttpHandler
  ): import("rxjs").Observable<import("@angular/common/http").HttpEvent<any>> {
    return next.handle(req).pipe(catchError(error => {
      if (error.status === 401)
        return throwError(error.statusText);

      //the httpErrorResponse refer the error that not handle by api sending status code
      //whihch just send it in response header
      if (error instanceof HttpErrorResponse) {
        //check if application-error exist in header,

        const applicationError = error.headers.get("Application-Error");

        //500 error
        //application-error only when there is an exception occur in api
        if (applicationError)
          return throwError(applicationError);

        //model state error
        //model state error won't intercept by the middleware of api
        //since it handle by .net itself.
        const serverError = error.error;
        let modalStateError = "";

        if (serverError.errors && typeof serverError.errors === "object") {
          for (const key in serverError.errors) {
            if (serverError.errors[key])
              modalStateError += serverError.errors[key] + "\n";
          }

        }

        return throwError(modalStateError || serverError || "Server error");
      }
    })
    );
  }

}

export const ErrorInterceptorProvider = {
  provide: HTTP_INTERCEPTORS,
  useClass: ErrorInterceptor,
  multi: true
};

