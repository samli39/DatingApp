import { Component } from '@angular/core';
import { ValuesService } from 'src/services/values.service';


@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})
export class AppComponent {
  title = 'DatingUI';
  valuesList: object;


  constructor(private service: ValuesService) {
  }

  ngOnInit() {
    this.service.GetAll().subscribe(data => {
      this.valuesList = data;
    })
  }
}
