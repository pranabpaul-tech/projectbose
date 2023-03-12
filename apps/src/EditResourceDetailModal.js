import React,{Component} from 'react';
import {Modal, Button, Row, Col, Form} from 'react-bootstrap';

export class EditResourceDetailModal extends Component{
    constructor(props){
        super(props);
        this.handleSubmit=this.handleSubmit.bind(this);
    }

    handleSubmit(event){
        event.preventDefault();
        fetch(process.env.REACT_APP_API+'Resourcedetails',{
            method:'PUT',
            headers:{
                'Accept':'application/json'
            },
            body:"{resourcedetailid:" + event.target.ResourceDetailId.value +
            ",projectname:" + event.target.ProjectName.value +
            ",resourcegroupname:" + event.target.ResourceGroupName.value +
            ",subscriptionid:" + event.target.SubscriptionId.value +
            ",projectowneremail:" + event.target.ProjectOwnerEmail.value +
            ",leveldetailid:" + event.target.LevelDetailId.value + "}"
        })
        .then(res=>res.json())
        .then((result)=>{
            alert(JSON.stringify(result));
        },
        (error)=>{
            alert('Failed');
        })
    }
    render(){
        return (
            <div className="container">

<Modal
{...this.props}
size="lg"
aria-labelledby="contained-modal-title-vcenter"
centered
>
    <Modal.Header closeButton>
        <Modal.Title id="contained-modal-title-vcenter">
            Edit ResourceDetail
        </Modal.Title>
    </Modal.Header>
    <Modal.Body>

        <Row>
            <Col sm={6}>
                <Form onSubmit={this.handleSubmit}>
                <Form.Group controlId="ResourceDetailId">
                        <Form.Label>ResourceDetailId</Form.Label>
                        <Form.Control type="text" name="ResourceDetailId" required 
                        placeholder="ResourceDetailId"/>
                    </Form.Group>
                    <Form.Group controlId="ProjectName">
                        <Form.Label>ProjectName</Form.Label>
                        <Form.Control type="text" name="ProjectName" required 
                        placeholder="ProjectName"/>
                    </Form.Group>
                    <Form.Group controlId="ResourceGroupName">
                        <Form.Label>ResourceGroupName</Form.Label>
                        <Form.Control type="text" name="ResourceGroupName" required 
                        placeholder="ResourceGroupName"/>
                    </Form.Group>
                    <Form.Group controlId="SubscriptionId">
                        <Form.Label>SubscriptionId</Form.Label>
                        <Form.Control type="text" name="SubscriptionId" required 
                        placeholder="SubscriptionId"/>
                    </Form.Group>
                    <Form.Group controlId="ProjectOwnerEmail">
                        <Form.Label>ProjectOwnerEmail</Form.Label>
                        <Form.Control type="text" name="ProjectOwnerEmail" required 
                        placeholder="ProjectOwnerEmail"/>
                    </Form.Group>
                    <Form.Group controlId="LevelDetailId">
                        <Form.Label>LevelDetailId</Form.Label>
                        <Form.Control type="text" name="LevelDetailId" required 
                        placeholder="LevelDetailId"/>
                    </Form.Group>
                    <Form.Group>
                        <Button variant="primary" type="submit">
                            Update Level
                        </Button>
                    </Form.Group>
                </Form>
            </Col>
        </Row>
    </Modal.Body>
    
    <Modal.Footer>
        <Button variant="danger" onClick={this.props.onHide}>Close</Button>
    </Modal.Footer>

</Modal>

            </div>
        )
    }

}