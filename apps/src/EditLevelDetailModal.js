import React,{Component} from 'react';
import {Modal, Button, Row, Col, Form} from 'react-bootstrap';

export class EditLevelDetailModal extends Component{
    constructor(props){
        super(props);
        this.handleSubmit=this.handleSubmit.bind(this);
    }

    handleSubmit(event){
        event.preventDefault();
        fetch(process.env.REACT_APP_API+'Leveldetails',{
            method:'PUT',
            headers:{
                'Accept':'application/json'
            },
            body:"{leveldetailid:" + event.target.LevelDetailId.value +
            ",leveldetailname:" + event.target.LevelDetailName.value +
            ",levelid:" + event.target.LevelId.value +
            ",superleveldetailid:" + event.target.SuperLevelDetailId.value + "}"
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
            Edit LevelDetail
        </Modal.Title>
    </Modal.Header>
    <Modal.Body>

        <Row>
            <Col sm={6}>
                <Form onSubmit={this.handleSubmit}>
                    <Form.Group controlId="LevelDetailId">
                        <Form.Label>LevelDetailId</Form.Label>
                        <Form.Control type="text" name="LevelDetailId" required
                        disabled
                        defaultValue={this.props.leveldetailid} 
                        placeholder="LevelDetailId"/>
                    </Form.Group>

                    <Form.Group controlId="LevelDetailName">
                        <Form.Label>LevelDetailName</Form.Label>
                        <Form.Control type="text" name="LevelDetailName" required 
                        defaultValue={this.props.levelname}
                        placeholder="LevelDetailName"/>
                    </Form.Group>

                    <Form.Group controlId="LevelId">
                        <Form.Label>LevelId</Form.Label>
                        <Form.Control type="text" name="LevelId" required
                        disabled
                        defaultValue={this.props.levelid} 
                        placeholder="LevelId"/>
                    </Form.Group>

                    <Form.Group controlId="SuperLevelDetailId">
                        <Form.Label>SuperLevelDetailId</Form.Label>
                        <Form.Control type="text" name="SuperLevelDetailId" required
                        defaultValue={this.props.superlevelid} 
                        placeholder="SuperLevelDetailId"/>
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