import { Component, OnInit, signal } from '@angular/core';
import { RecipeService } from '../services/recipe.service';
import { RecipeModel } from '../models/recipe';
import { Recipe } from '../recipe/recipe';

@Component({
  selector: 'app-recipes',
  standalone: true,
  imports: [Recipe],
  templateUrl: './recipes.html',
  styleUrl: './recipes.css'
})
export class Recipes implements OnInit {
  protected recipes = signal<RecipeModel[]>([]);

  constructor(private recipeService: RecipeService) {}

  ngOnInit(): void {
    this.recipeService.getAllRecipes().subscribe({
      next: (data:any) => {
        this.recipes.set(data.recipes);
        // console.log(data);
      },

      error: (err:any) => {
        console.error(err);
      }
    });
  }
}
