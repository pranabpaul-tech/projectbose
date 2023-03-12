import React,{Component} from 'react';
import {Table} from 'react-bootstrap';

import {Button,ButtonToolbar} from 'react-bootstrap';
import {AddResourceDetailModal} from './AddResourceDetailModal';
import {EditResourceDetailModal} from './EditResourceDetailModal';

export class ResourceDetails extends Component{

    constructor(props){
        super(props);
        this.state={resourcedetails:[], addModalShow:false, editModalShow:false}
    }

    refreshList(){
        let url = process.env.REACT_APP_API+'Resourcedetails';
        let config = {
        method: "GET",
        cache: 'no-cache'
        };
        fetch(url, config)
        //fetch(process.env.REACT_APP_API+'ResourceDetails')
        .then(response=>response.json())
        .then(data=>{
            this.setState({resourcedetails:data});
        });
        
    }

    componentDidMount(){
        this.refreshList();
    }

    componentDidUpdate(){
        this.refreshList();
    }

    deleteResourceDetail(resourcedetailid){
        if(window.confirm('Are you sure?')){
            fetch(process.env.REACT_APP_API+'Resourcedetails/'+resourcedetailid,{
                method:'DELETE',
                header:{'Accept':'application/json',
            'Content-Type':'application/json'}
            })
        }
    }
    render(){
        const {resourcedetails, resourcedetailid,projectname,resourcegroupname,subscriptionid,projectowneremail,leveldetailid}=this.state;
        let addModalClose=()=>this.setState({addModalShow:false});
        let editModalClose=()=>this.setState({editModalShow:false});
        return(
            <div >
                <Table className="mt-4" striped bordered hover size="sm">
                    <thead>
                        <tr>
                            <th>ResourceDetailId</th>
                            <th>ProjectName</th>
                            <th>ResourceGroupName</th>
                            <th>SubscriptionId</th>
                            <th>ProjectOwnerEmail</th>
                            <th>LevelDetailId</th>
                            <th>Options</th>
                        </tr>
                    </thead>
                    <tbody>
                        {resourcedetails.map(resourcedetail=>
                            <tr key={resourcedetail.resourcedetailid}>
                                <td>{resourcedetail.resourcedetailid}</td>
                                <td>{resourcedetail.projectname}</td>
                                <td>{resourcedetail.resourcegroupname}</td>
                                <td>{resourcedetail.subscriptionid}</td>
                                <td>{resourcedetail.projectowneremail}</td>
                                <td>{resourcedetail.leveldetailid}</td>
                                <td>
<ButtonToolbar>
    <Button className="mr-2" variant="info"
    onClick={()=>this.setState({editModalShow:true,
        resourcedetailid:resourcedetail.resourcedetailid,projectname:resourcedetail.projectname,resourcegroupname:resourcedetail.resourcegroupname,subscriptionid:resourcedetail.subscriptionid,projectowneremail:resourcedetail.projectowneremail,leveldetailid:resourcedetail.leveldetailid})}>
            Edit
        </Button>

        <Button className="mr-2" variant="danger"
    onClick={()=>this.deleteResourceDetail(resourcedetail.resourcedetailid)}>
            Delete
        </Button>

        <EditResourceDetailModal show={this.state.editModalShow}
        onHide={editModalClose}
        resourcedetailid={resourcedetailid}
        projectname={projectname}
        resourcegroupname={resourcegroupname}
        subscriptionid={subscriptionid}
        projectowneremail={projectowneremail}
        leveldetailid={leveldetailid}/>
</ButtonToolbar>

                                </td>

                            </tr>)}
                    </tbody>

                </Table>

                <ButtonToolbar>
                    <Button variant='primary'
                    onClick={()=>this.setState({addModalShow:true})}>
                    Add ResourceDetail</Button>

                    <AddResourceDetailModal show={this.state.addModalShow}
                    onHide={addModalClose}/>
                </ButtonToolbar>
            </div>
        )
    }
}