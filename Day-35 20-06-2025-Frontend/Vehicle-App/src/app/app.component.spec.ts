import { ComponentFixture, TestBed } from '@angular/core/testing';
import { AppComponent } from './app.component';
import { Component } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { HttpClientTestingModule } from '@angular/common/http/testing';

// ✅ Mock NavBarComponent
@Component({
  selector: 'app-nav-bar',
  standalone: true,
  template: '<div>Mock NavBar</div>'
})
class MockNavBarComponent {}

// ✅ Mock FooterComponent
@Component({
  selector: 'app-footer',
  standalone: true,
  template: '<div>Mock Footer</div>'
})
class MockFooterComponent {}

describe('AppComponent', () => {
  let fixture: ComponentFixture<AppComponent>;
  let component: AppComponent;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [
        AppComponent,
        RouterOutlet,
        HttpClientTestingModule,
        MockNavBarComponent,
        MockFooterComponent
      ]
    }).compileComponents();

    fixture = TestBed.createComponent(AppComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create the AppComponent', () => {
    expect(component).toBeTruthy();
  });

  it('should have title "AutoCare"', () => {
    expect(component.title).toBe('AutoCare');
  });

  it('should render NavBar and Footer components', () => {
    const compiled = fixture.nativeElement as HTMLElement;
    expect(compiled.textContent).toContain('Mock NavBar');
    expect(compiled.textContent).toContain('Mock Footer');
  });
});
