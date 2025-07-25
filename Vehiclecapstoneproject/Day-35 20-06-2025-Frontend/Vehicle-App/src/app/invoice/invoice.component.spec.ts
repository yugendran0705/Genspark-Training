import { ComponentFixture, TestBed, fakeAsync, tick } from '@angular/core/testing';
import { InvoiceComponent } from './invoice.component';
import { ActivatedRoute } from '@angular/router';
import { of, throwError } from 'rxjs';
import { InvoiceService } from '../services/invoice.service';
import { HttpClientTestingModule } from '@angular/common/http/testing';
import { ReactiveFormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';

describe('InvoiceComponent', () => {
  let component: InvoiceComponent;
  let fixture: ComponentFixture<InvoiceComponent>;
  let mockInvoiceService: jasmine.SpyObj<InvoiceService>;

  beforeEach(async () => {
    mockInvoiceService = jasmine.createSpyObj('InvoiceService', ['getInvoiceByBookingID', 'getInvoicePDFByBookingID']);

    await TestBed.configureTestingModule({
      imports: [
        InvoiceComponent,
        HttpClientTestingModule,
        ReactiveFormsModule,
        CommonModule
      ],
      providers: [
        {
          provide: ActivatedRoute,
          useValue: {
            snapshot: {
              paramMap: {
                get: () => '123' // simulate route param id = 123
              }
            }
          }
        },
        { provide: InvoiceService, useValue: mockInvoiceService }
      ]
    }).compileComponents();

    fixture = TestBed.createComponent(InvoiceComponent);
    component = fixture.componentInstance;
  });

  it('should create the component', () => {
    expect(component).toBeTruthy();
  });

  it('should fetch invoice on init if id exists', fakeAsync(() => {
    const mockInvoice = { id: 1, bookingId: 123 };
    mockInvoiceService.getInvoiceByBookingID.and.returnValue(of(mockInvoice));

    component.ngOnInit();
    tick();
    expect(component.invoice).toEqual(mockInvoice);
    expect(component.loading).toBeFalse();
    expect(component.error).toBeNull();
  }));

  it('should set error if invoice fetch fails', fakeAsync(() => {
    mockInvoiceService.getInvoiceByBookingID.and.returnValue(throwError(() => ({ error: 'Not found' })));

    component.ngOnInit();
    tick();

    expect(component.invoice).toBeNull();
    expect(component.loading).toBeFalse();
    expect(component.error).toBe('Failed to fetch invoice.');
  }));

  it('should download PDF when downloadPDF() is called', fakeAsync(() => {
    const mockBlob = new Blob(['mock pdf'], { type: 'application/pdf' });
    const mockInvoice = { id: 456, bookingId: 123 };
    component.invoice = mockInvoice;

    mockInvoiceService.getInvoicePDFByBookingID.and.returnValue(of(mockBlob));

    // spy on createElement and appendChild/removeChild
    const appendSpy = spyOn(document.body, 'appendChild').and.callThrough();
    const removeSpy = spyOn(document.body, 'removeChild').and.callThrough();
    const revokeSpy = spyOn(window.URL, 'revokeObjectURL').and.callThrough();

    component.downloadPDF();
    tick();

    expect(mockInvoiceService.getInvoicePDFByBookingID).toHaveBeenCalledWith('123');
    expect(appendSpy).toHaveBeenCalled();
    expect(removeSpy).toHaveBeenCalled();
    expect(revokeSpy).toHaveBeenCalled();
  }));

  it('should not try to download PDF if invoice is null or missing bookingId', () => {
    component.invoice = null;

    component.downloadPDF();

    expect(mockInvoiceService.getInvoicePDFByBookingID).not.toHaveBeenCalled();
  });

  it('should alert user if download fails', fakeAsync(() => {
    spyOn(window, 'alert');
    component.invoice = { id: 789, bookingId: 123 };
    mockInvoiceService.getInvoicePDFByBookingID.and.returnValue(throwError(() => new Error('Download failed')));

    component.downloadPDF();
    tick();

    expect(window.alert).toHaveBeenCalledWith('Failed to download invoice. Please try again.');
  }));
});
