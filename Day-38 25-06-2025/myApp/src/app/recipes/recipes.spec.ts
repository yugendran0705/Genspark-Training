import { ComponentFixture, TestBed } from '@angular/core/testing';
import { RecipeService } from '../services/recipeservice';
import { of, throwError } from 'rxjs';
import { Recipe } from '../recipe/recipe';
import { CUSTOM_ELEMENTS_SCHEMA } from '@angular/core';
import { Recipes } from './recipes';

describe('Recipe Component', () => {
  let component: Recipes;
  let fixture: ComponentFixture<Recipes>;
  let servicespy: jasmine.SpyObj<RecipeService>;

  const mockResponse = {
    recipes: [
      {
        id: 1,
        name: 'Biryani',
        cuisine: 'Indian',
        cookTimeMinutes: 60,
        ingredients: ['Rice', 'Chicken', 'Spices'],
        image: 'https://example.com/biryani.jpg'
      }
    ]
  };

  beforeEach(() => {
    servicespy = jasmine.createSpyObj('RecipeService', ['getAllRecipes'])

    TestBed.configureTestingModule({
      imports: [Recipe],
      providers: [
        { provide: RecipeService, useValue: servicespy }
      ],
      schemas: [CUSTOM_ELEMENTS_SCHEMA]
    });

    fixture = TestBed.createComponent(Recipes)
    component = fixture.componentInstance;
    servicespy = TestBed.inject(RecipeService) as jasmine.SpyObj<RecipeService>;


  });

  it('should be created', () => {
    expect(component).toBeTruthy();
  });

  it('should call getAllRecipes and return data', (done) => {
    servicespy.getAllRecipes.and.returnValue(of(mockResponse));

    servicespy.getAllRecipes().subscribe((data: any) => {
      expect(data).toEqual(mockResponse);
      expect(servicespy.getAllRecipes).toHaveBeenCalled();
      done();
    });
  });

  it('should handle error from getAllRecipes', (done) => {
    const error = new Error('Network error');
    servicespy.getAllRecipes.and.returnValue(throwError(() => error));

    servicespy.getAllRecipes().subscribe({
      next: () => { },
      error: (err: any) => {
        expect(err).toBe(error);
        done();
      }
    });
  });
});