import { Component, Input } from '@angular/core';
import { RecipeModel } from '../models/Recipe';

@Component({
  selector: 'app-recipe',
  standalone: true,
  templateUrl: './recipe.html',
  styleUrl: './recipe.css'
})
export class Recipe {
  @Input() recipe: RecipeModel | null = null;
}
