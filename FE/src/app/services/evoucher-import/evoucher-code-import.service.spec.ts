import { TestBed } from '@angular/core/testing';

import { EvoucherCodeImportService } from './evoucher-code-import.service';

describe('EvoucherCodeImportService', () => {
  beforeEach(() => TestBed.configureTestingModule({}));

  it('should be created', () => {
    const service: EvoucherCodeImportService = TestBed.get(EvoucherCodeImportService);
    expect(service).toBeTruthy();
  });
});
