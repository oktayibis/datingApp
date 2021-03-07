import {Component, OnInit} from '@angular/core';
import {HttpClient} from '@angular/common/http';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})
export class AppComponent implements OnInit{

  users: any;
  constructor(private http: HttpClient) {
  }

  ngOnInit(): void {
    this.getUsers();
  }

  // tslint:disable-next-line:typedef
  getUsers() {
    this.http.get('https://localhost:5001/api/users')
      .subscribe(response => {
        this.users = response;
      }, error => console.log(error));
  }
}
