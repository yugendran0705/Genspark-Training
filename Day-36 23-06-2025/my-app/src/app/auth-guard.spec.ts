import { TestBed } from '@angular/core/testing';
import { Router, ActivatedRouteSnapshot, RouterStateSnapshot } from '@angular/router';
import { AuthGuard } from './auth-guard';
// Import jasmine types for SpyObj
import 'jasmine';

describe('AuthGuard', () => {
  let authGuard: AuthGuard;
  let router: jasmine.SpyObj<Router>;

  // Dummy snapshots for the canActivate call; adjust them as needed.
  const dummyActivatedRouteSnapshot = {} as ActivatedRouteSnapshot;
  const dummyRouterStateSnapshot = {} as RouterStateSnapshot;

  beforeEach(() => {
    // Create a Jasmine spy object for Router with a 'navigate' method.
    const routerSpy = jasmine.createSpyObj('Router', ['navigate']);

    TestBed.configureTestingModule({
      providers: [
        AuthGuard,
        { provide: Router, useValue: routerSpy }
      ]
    });

    authGuard = TestBed.inject(AuthGuard);
    // Explicitly assert the type for TypeScript.
    router = TestBed.inject(Router) as jasmine.SpyObj<Router>;
  });

  // Clean up localStorage after each test to avoid state bleed.
  afterEach(() => {
    localStorage.removeItem('token');
  });

  it('should allow activation when a token is present in localStorage', () => {
    // Arrange: set a dummy token.
    localStorage.setItem('token', 'dummy-token');

    // Act: call the guard.
    const result = authGuard.canActivate(dummyActivatedRouteSnapshot, dummyRouterStateSnapshot);

    // Assert: should return true and not trigger a redirect.
    expect(result).toBeTrue();
    expect(router.navigate).not.toHaveBeenCalled();
  });

  it('should block activation and navigate to "login" when no token is found', () => {
    // Arrange: ensure no token is present.
    localStorage.removeItem('token');

    // Act: call the guard.
    const result = authGuard.canActivate(dummyActivatedRouteSnapshot, dummyRouterStateSnapshot);

    // Assert: should return false and call router.navigate with ['login'].
    expect(result).toBeFalse();
    expect(router.navigate).toHaveBeenCalledWith(['login']);
  });
});
