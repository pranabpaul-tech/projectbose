import React,{Component} from 'react';
import {Button} from 'react-bootstrap';

export class Home extends Component{

    render(){
        return(
            <div>
            <div className="mt-5 d-flex justify-content-left">
              Click this button after Add/Edit of Levels and LevelDetails is done.
            </div>
            <div className="mt-5 d-flex justify-content-left">
            <Button variant="danger">Format Levels</Button>
           </div>
           <div className="mt-5 d-flex justify-content-left">
           Click this button create/re-create Azure Cost Detail data.
            </div>
            <div className="mt-5 d-flex justify-content-left">
            <Button variant="danger">Format Final Data</Button>
           </div>
           </div>
        )
    }
}