import { HttpInterceptorFn } from '@angular/common/http';
import { of, switchMap } from 'rxjs';


/**
 * HTTP Interceptor for Angular 20+ (functional style)
 * Automatically adds Firebase ID token to all HTTP requests
 */
export const authInterceptor: HttpInterceptorFn = (req, next) => {
  const token = localStorage.getItem('toxin');
  
  if (token) {
    const clonedRequest = req.clone({
      setHeaders: {
        Authorization: `Bearer ${token}`
      }
    });
    return next(clonedRequest);
  }

  return next(req);
};