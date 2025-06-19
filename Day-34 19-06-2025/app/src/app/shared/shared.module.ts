import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule } from '@angular/forms';
import { MatSnackBarModule } from '@angular/material/snack-bar';

import { UserListComponent } from '../user-list/user-list.component';
import { UserSearchComponent } from '../user-search/user-search.component';
import { UserFormComponent } from '../user-form/user-form.component';

@NgModule({
  declarations: [
  ],
  imports: [
    UserListComponent,
    UserSearchComponent,
    UserFormComponent,
    CommonModule,
    ReactiveFormsModule,
    MatSnackBarModule
  ],
  exports: [
    CommonModule,
    ReactiveFormsModule,
    MatSnackBarModule,
    UserListComponent,
    UserSearchComponent
  ]
})
export class SharedModule {}
