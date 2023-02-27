import React,{Component} from 'react';
import {Table} from 'react-bootstrap';

import {Button,ButtonToolbar} from 'react-bootstrap';
import {AddLevelModal} from './AddLevelModal';
import {EditLevelModal} from './EditLevelModal';

export class Levels extends Component{

    constructor(props){
        super(props);
        this.state={levels:[], addModalShow:false, editModalShow:false}
    }

    refreshList(){
        let url = process.env.REACT_APP_API+'Levels';
        let config = {
        method: "GET",
        cache: 'no-cache'
        };
        fetch(url, config)
        //fetch(process.env.REACT_APP_API+'Levels')
        .then(response=>response.json())
        .then(data=>{
            this.setState({levels:data});
        });
        
    }

    componentDidMount(){
        this.refreshList();
    }

    componentDidUpdate(){
        this.refreshList();
    }

    deleteLevel(levelid){
        if(window.confirm('Are you sure?')){
            const requestOptions = {method: 'DELETE'};
            fetch(process.env.REACT_APP_API+'Levels/'+levelid,requestOptions);
        }
    }
    render(){
        const {levels, levelid,levelname,superlevelid}=this.state;
        let addModalClose=()=>this.setState({addModalShow:false});
        let editModalClose=()=>this.setState({editModalShow:false});
        return(
            <div >
                <Table className="mt-4" striped bordered hover size="sm">
                    <thead>
                        <tr>
                            <th>LevelId</th>
                            <th>LevelName</th>
                            <th>SuperLevelId</th>
                            <th>Options</th>
                        </tr>
                    </thead>
                    <tbody>
                        {levels.map(level=>
                            <tr key={level.levelid}>
                                <td>{level.levelid}</td>
                                <td>{level.levelname}</td>
                                <td>{level.superlevelid}</td>
                                <td>
<ButtonToolbar>
    <Button className="mr-2" variant="info"
    onClick={()=>this.setState({editModalShow:true,
        levelid:level.levelid,levelname:level.levelName,superlevelid:level.superlevelid})}>
            Edit
        </Button>

        <Button className="mr-2" variant="danger"
    onClick={()=>this.deleteLevel(level.levelid)}>
            Delete
        </Button>

        <EditLevelModal show={this.state.editModalShow}
        onHide={editModalClose}
        levelid={levelid}
        levelname={levelname}
        superlevelid={superlevelid}/>
</ButtonToolbar>

                                </td>

                            </tr>)}
                    </tbody>

                </Table>

                <ButtonToolbar>
                    <Button variant='primary'
                    onClick={()=>this.setState({addModalShow:true})}>
                    Add Level</Button>

                    <AddLevelModal show={this.state.addModalShow}
                    onHide={addModalClose}/>
                </ButtonToolbar>
            </div>
        )
    }
}