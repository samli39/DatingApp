import { Component, OnInit, Output, EventEmitter } from '@angular/core';
import { AuthService } from 'src/services/auth/auth.service';


@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.scss']
})
export class RegisterComponent implements OnInit {
  model: any = {}
  @Output() cancelRegister = new EventEmitter();
  constructor(private auth: AuthService) { }

  ngOnInit() {
  }

  register() {
    this.auth.register(this.model).subscribe(data => {
      console.log("registration done");
    }, err => {
      console.log(err);
    })
  }

  cancel() {
    this.cancelRegister.emit(false);
  }

}
