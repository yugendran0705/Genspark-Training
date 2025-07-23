import { ComponentFixture, TestBed } from '@angular/core/testing';
import { FooterComponent } from './footer.component';

describe('FooterComponent', () => {
  let component: FooterComponent;
  let fixture: ComponentFixture<FooterComponent>;
  let compiled: HTMLElement;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [FooterComponent],
    }).compileComponents();

    fixture = TestBed.createComponent(FooterComponent);
    component = fixture.componentInstance;
    compiled = fixture.nativeElement;
    fixture.detectChanges();
  });

  it('should create the FooterComponent', () => {
    expect(component).toBeTruthy();
  });

  it('should render the copyright',
    () => {
      const paragraph = compiled.querySelector('p');
      expect(paragraph?.textContent).toContain('© 2025 AutoCare');
    });

  it('should have 3 footer links: Privacy, Terms, Contact', () => {
    const links = compiled.querySelectorAll('a');
    const linkTexts = Array.from(links).map(link => link.textContent?.trim());
    expect(linkTexts).toEqual(['Privacy', 'Terms', 'Contact']);
  });

  it('should apply hover:text-white class to links', () => {
    const links = compiled.querySelectorAll('a');
    links.forEach(link => {
      expect(link.classList).toContain('hover:text-white');
    });
  });

  it('should have expected layout classes for responsiveness', () => {
    const footer = compiled.querySelector('footer');
    expect(footer?.classList).toContain('bg-gray-800');
    expect(footer?.classList).toContain('text-gray-400');
  });
});
