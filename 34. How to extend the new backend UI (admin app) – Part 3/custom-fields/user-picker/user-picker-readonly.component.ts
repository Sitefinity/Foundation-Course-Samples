import { Component, OnInit } from "@angular/core";
import { FieldBase } from "@progress/sitefinity-adminapp-sdk/app/api/v1";
import { User } from "./user";

/**
 * The component used to display the field in read only mode.
 * One can use inline template & styles OR templateUrl & styleUrls OR a mixture of that like here. See the -write.component.ts version for the write mode type.
 */
@Component({
    templateUrl: "./user-picker-readonly.component.html",
    styleUrls: [ "./user.component.css" ]
})
export class UserPickerReadonlyComponent extends FieldBase implements OnInit { 
    user: User | null;

    get parsedValue(){
        if(this.getValue()){
            return JSON.parse(this.getValue())
        }

        return null;
    }

    ngOnInit(){
        this.user = this.parsedValue;
    }
}
