import { EVoucherImportModule } from './e-voucher-import.module';

describe('EVoucherImportModule', () => {
  let eVoucherImportModule: EVoucherImportModule;

  beforeEach(() => {
    eVoucherImportModule = new EVoucherImportModule();
  });

  it('should create an instance', () => {
    expect(eVoucherImportModule).toBeTruthy();
  });
});
