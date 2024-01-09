import { TestBed } from '@angular/core/testing';

import { AuthenticationUserService } from './authentication-user.service';

describe('AuthenticationUserService', () => {
  let service: AuthenticationUserService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(AuthenticationUserService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
