import { TestBed } from '@angular/core/testing';

import { VscService } from './vsc.service';

describe('VscService', () => {
  beforeEach(() => TestBed.configureTestingModule({}));

  it('should be created', () => {
    const service: VscService = TestBed.get(VscService);
    expect(service).toBeTruthy();
  });
});
