import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { catchError, Observable, throwError } from 'rxjs';
import { map } from 'rxjs/operators';

@Injectable({
  providedIn: 'root'
})
export class RecipeService {
  private http = inject(HttpClient);

//   getAllRecipes(): Observable<any[]> {
//     return this.http.get('/recipe.csv', { responseType: 'text' }).pipe(
//       catchError(error => throwError(() => error)),
//       map(text => {
//         const lines = text.split('\n').filter(line => line.trim().length > 0);

//         const headers = lines[0].split(',').map(h => h.trim());

//         return lines.slice(1).map((line, idx) => {
//           const values = line.split(',').map(v => v.trim());
//           const obj: any = { id: idx };
//           headers.forEach((header, i) => {
//             if (header === "Ingredients"){
//                 const value = values[i].split('&').map(v => " "+v.trim());
//                 obj[header] = value
//             }
//             else obj[header] = values[i];
//           });
//           return obj;
//         });
//       })
//     );
//   }
    
    getAllRecipes():Observable<any[]>{
        return this.http.get<any[]>('https://dummyjson.com/recipes');
    }
}
