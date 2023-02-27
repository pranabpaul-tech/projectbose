import React,{Component} from 'react';
import {Button} from 'react-bootstrap';

export class Home extends Component{

    prepareLevel(){
        if(window.confirm('Are you sure?')){
            const requestOptions = {method: 'POST'};
            fetch(process.env.REACT_APP_API+'Preparedata/',requestOptions);
        }
    }

    prepareAzureCost(){
        if(window.confirm('Are you sure?')){
            const requestOptions = {method: 'PUT'};
            fetch(process.env.REACT_APP_API+'Preparedata/',requestOptions);
        }
    }

    render(){
        return(
            <div>
            <div className="mt-5 d-flex justify-content-left">
              Click this button after Add/Edit of Levels and LevelDetails is done.
            </div>
            <div className="mt-5 d-flex justify-content-left">
            <Button variant="danger" onClick={()=>this.prepareLevel()}>Format Levels</Button>
           </div>
           <div className="mt-5 d-flex justify-content-left">
           Click this button create/re-create Azure Cost Detail data.
            </div>
            <div className="mt-5 d-flex justify-content-left">
            <Button variant="danger" onClick={()=>this.prepareAzureCost()}>Format Final Data</Button>
           </div>
           </div>
        )
    }
}