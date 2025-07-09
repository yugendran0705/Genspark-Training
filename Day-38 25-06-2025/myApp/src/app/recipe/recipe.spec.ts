import { ComponentFixture, TestBed } from '@angular/core/testing';
import { Recipe } from '../recipe/recipe';
import { RecipeModel } from '../models/Recipe';

describe('Recipe Component', () => {
  let component: Recipe;
  let fixture: ComponentFixture<Recipe>;

  const mockRecipe: RecipeModel = {
    id: 1,
    name: 'Butter Chicken',
    cuisine: 'Indian',
    cookTimeMinutes: 45,
    ingredients: ['Chicken', 'Butter', 'Cream', 'Spices'],
    image: 'https://example.com/image.jpg'
  };

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [Recipe] 
    }).compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(Recipe);
    component = fixture.componentInstance;
    component.recipe = mockRecipe;
    fixture.detectChanges();
  });

  it('should create the Recipe component', () => {
    expect(component).toBeTruthy();
  });
    it('should display the cuisine type', () => {
    const text = fixture.nativeElement.textContent;
    expect(text).toContain('Indian');
  });

  it('should display the cook time', () => {
    const text = fixture.nativeElement.textContent;
    expect(text).toContain('45');
  });

});
