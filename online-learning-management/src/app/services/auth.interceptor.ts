import { HttpInterceptorFn } from '@angular/common/http';

export const authInterceptor: HttpInterceptorFn = (req, next) => {
  console.log('Interceptor is active');

  const token = localStorage.getItem('token'); // Aici este stocat token-ul JWT

  if (token) {
    console.log(`Interceptor: Token found: Bearer ${token}`);
    // Clonează cererea și adaugă antetul Authorization
    req = req.clone({
      setHeaders: {
        Authorization: `Bearer ${token}`
      }
    });
  } else {
    console.error('Interceptor: No token found in localStorage');
  }

  console.log('Interceptor: Request headers:', req.headers);

  return next(req); // Continuă cererea cu modificările
};

