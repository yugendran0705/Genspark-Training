import { ComponentFixture, TestBed, fakeAsync, tick } from '@angular/core/testing';
import { BookingComponent } from './booking.component';
import { of, throwError } from 'rxjs';
import { BookingService } from '../services/book.service';
import { ImageService } from '../services/image.service';
import { Router, ActivatedRoute } from '@angular/router';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule } from '@angular/forms';

describe('BookingComponent', () => {
  let component: BookingComponent;
  let fixture: ComponentFixture<BookingComponent>;
  let mockBookingService: jasmine.SpyObj<BookingService>;
  let mockImageService: jasmine.SpyObj<ImageService>;
  let mockRouter: jasmine.SpyObj<Router>;
  let mockActivatedRoute: ActivatedRoute;

  beforeEach(async () => {
    mockBookingService = jasmine.createSpyObj('BookingService', ['getOne']);
    mockImageService = jasmine.createSpyObj('ImageService', ['getAllByBookingID', 'uploadImage', 'deleteImage']);
    mockRouter = jasmine.createSpyObj('Router', ['navigate']);

    mockActivatedRoute = {
      paramMap: of({
        get: () => '123'
      })
    } as any;

    await TestBed.configureTestingModule({
      imports: [CommonModule, ReactiveFormsModule, BookingComponent],
      providers: [
        { provide: BookingService, useValue: mockBookingService },
        { provide: ImageService, useValue: mockImageService },
        { provide: Router, useValue: mockRouter },
        { provide: ActivatedRoute, useValue: mockActivatedRoute },
      ]
    }).compileComponents();

    fixture = TestBed.createComponent(BookingComponent);
    component = fixture.componentInstance;
  });

  it('should create the component', () => {
    expect(component).toBeTruthy();
  });

  it('should load booking and images on init', fakeAsync(() => {
    const mockBooking = { id: 123, vehicleId: 55 };
    const mockImages = [{ id: 1, data: 'img1' }, { id: 2, data: 'img2' }];

    mockBookingService.getOne.and.returnValue(of(mockBooking));
    mockImageService.getAllByBookingID.and.returnValue(of(mockImages));

    component.ngOnInit();
    tick(); // wait for subscriptions

    expect(component.booking).toEqual(mockBooking);
    expect(component.vehicleId).toBe(55);
    expect(component.images).toEqual(mockImages);
  }));

  it('should handle error when booking fetch fails', () => {
    mockBookingService.getOne.and.returnValue(throwError(() => ({ error: { error: 'Booking failed' } })));
    mockImageService.getAllByBookingID.and.returnValue(of([]));

    component.ngOnInit();

    expect(component.errorMessage).toBe('Booking failed');
  });

  it('should handle image navigation (next and prev)', () => {
    component.images = [{}, {}, {}];
    component.currentIndex = 0;

    component.next();
    expect(component.currentIndex).toBe(1);

    component.prev();
    expect(component.currentIndex).toBe(0);
  });

  it('should upload image if base64 is set', () => {
    component.base64Image = 'data:image/jpeg;base64,abc';
    component.bookingId = 123;
    component.vehicleId = 5;
    mockImageService.uploadImage.and.returnValue(of({ success: true }));

    component.uploadImage();

    expect(mockImageService.uploadImage).toHaveBeenCalledWith({
      Base64Data: 'data:image/jpeg;base64,abc',
      BookingId: 123,
      VehicleID: 5
    });
  });

  it('should delete image and update list', () => {
    component.images = [{ id: 1 }, { id: 2 }, { id: 3 }];
    component.currentIndex = 2;
    mockImageService.deleteImage.and.returnValue(of({}));

    component.deleteImage(1);

    expect(component.images.length).toBe(2);
    expect(component.images.find(i => i.id === 2)).toBeUndefined();
  });

  it('should navigate to invoice download', () => {
    component.booking = { id: 42 };
    component.downloadInvoice();
    expect(mockRouter.navigate).toHaveBeenCalledWith(['/invoice/42']);
  });

  it('should not upload if base64Image is empty', () => {
    component.base64Image = '';
    component.uploadImage();
    expect(mockImageService.uploadImage).not.toHaveBeenCalled();
  });

  it('should update base64Image on file change', fakeAsync(() => {
    const mockEvent = {
      target: {
        files: [new Blob(['image-data'], { type: 'image/jpeg' }) as File]
      }
    } as unknown as Event;

    const fileReaderSpy = jasmine.createSpyObj('FileReader', ['readAsDataURL']);
    fileReaderSpy.onload = () => {};
    (window as any).FileReader = function () {
      return fileReaderSpy;
    };

    component.onFileChange(mockEvent);
    expect(fileReaderSpy.readAsDataURL).toHaveBeenCalled();
  }));
});
