import React,{Component} from 'react';
import {Table} from 'react-bootstrap';

import {Button,ButtonToolbar} from 'react-bootstrap';
import {AddLevelDetailModal} from './AddLevelDetailModal';
import {EditLevelDetailModal} from './EditLevelDetailModal';

export class LevelDetails extends Component{

    constructor(props){
        super(props);
        this.state={leveldetails:[], addModalShow:false, editModalShow:false}
    }

    refreshList(){
        let url = process.env.REACT_APP_API+'LevelDetails';
        let config = {
        method: "GET",
        cache: 'no-cache'
        };
        fetch(url, config)
        //fetch(process.env.REACT_APP_API+'LevelDetails')
        .then(response=>response.json())
        .then(data=>{
            this.setState({leveldetails:data});
        });
        
    }

    componentDidMount(){
        this.refreshList();
    }

    componentDidUpdate(){
        this.refreshList();
    }

    deleteLevelDetail(leveldetailid){
        if(window.confirm('Are you sure?')){
            fetch(process.env.REACT_APP_API+'Leveldetails/'+leveldetailid,{
                method:'DELETE',
                header:{'Accept':'application/json',
            'Content-Type':'application/json'}
            })
        }
    }
    render(){
        const {leveldetails, leveldetailid,leveldetailname,levelid,superleveldetailid}=this.state;
        let addModalClose=()=>this.setState({addModalShow:false});
        let editModalClose=()=>this.setState({editModalShow:false});
        return(
            <div >
                <Table className="mt-4" striped bordered hover size="sm">
                    <thead>
                        <tr>
                            <th>LevelDetailId</th>
                            <th>LevelDetailName</th>
                            <th>LevelId</th>
                            <th>SuperLevelDetailId</th>
                            <th>Options</th>
                        </tr>
                    </thead>
                    <tbody>
                        {leveldetails.map(leveldetail=>
                            <tr key={leveldetail.leveldetailid}>
                                <td>{leveldetail.leveldetailid}</td>
                                <td>{leveldetail.leveldetailname}</td>
                                <td>{leveldetail.levelid}</td>
                                <td>{leveldetail.superleveldetailid}</td>
                                <td>
<ButtonToolbar>
    <Button className="mr-2" variant="info"
    onClick={()=>this.setState({editModalShow:true,
        leveldetailid:leveldetail.leveldetailid,leveldetailname:leveldetail.leveldetailname,levelid:leveldetail.levelid,superLeveldetailid:leveldetail.superleveldetailid})}>
            Edit
        </Button>

        <Button className="mr-2" variant="danger"
    onClick={()=>this.deleteLevelDetail(leveldetail.leveldetailid)}>
            Delete
        </Button>

        <EditLevelDetailModal show={this.state.editModalShow}
        onHide={editModalClose}
        leveldetailid={leveldetailid}
        leveldetailname={leveldetailname}
        levelid={levelid}
        superleveldetailid={superleveldetailid}/>
</ButtonToolbar>

                                </td>

                            </tr>)}
                    </tbody>

                </Table>

                <ButtonToolbar>
                    <Button variant='primary'
                    onClick={()=>this.setState({addModalShow:true})}>
                    Add LevelDetail</Button>

                    <AddLevelDetailModal show={this.state.addModalShow}
                    onHide={addModalClose}/>
                </ButtonToolbar>
            </div>
        )
    }
}