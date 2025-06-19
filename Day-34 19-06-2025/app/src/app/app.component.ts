import { Component } from '@angular/core';
import { SharedModule } from './shared/shared.module';
import { UserFormComponent } from "./user-form/user-form.component";
@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css'],
  imports: [SharedModule, UserFormComponent]
})
export class AppComponent {
  title = 'User Management';
}
