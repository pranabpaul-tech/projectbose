import React,{Component} from 'react';
import {Modal, Button, Row, Col, Form} from 'react-bootstrap';

export class EditLevelModal extends Component{
    constructor(props){
        super(props);
        this.handleSubmit=this.handleSubmit.bind(this);
    }

    handleSubmit(event){
        event.preventDefault();

        const requestOptions = {
            method: 'PUT',
            body: JSON.stringify({levelid: event.target.LevelId.value,levelname: event.target.LevelName.value,superlevelid: event.target.SuperLevelId.value})
        };
        fetch(process.env.REACT_APP_API+'Levels', requestOptions)
            .then(response => response.json())
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
            Edit Level
        </Modal.Title>
    </Modal.Header>
    <Modal.Body>

        <Row>
            <Col sm={6}>
                <Form onSubmit={this.handleSubmit}>
                <Form.Group controlId="LevelId">
                        <Form.Label>LevelId</Form.Label>
                        <Form.Control type="text" name="LevelId" required
                        disabled
                        defaultValue={this.props.levelid} 
                        placeholder="LevelId"/>
                    </Form.Group>

                    <Form.Group controlId="LevelName">
                        <Form.Label>LevelName</Form.Label>
                        <Form.Control type="text" name="LevelName" required 
                        defaultValue={this.props.levelname}
                        placeholder="LevelName"/>
                    </Form.Group>

                    <Form.Group controlId="SuperLevelId">
                        <Form.Label>SuperLevelId</Form.Label>
                        <Form.Control type="text" name="SuperLevelId" required
                        defaultValue={this.props.superlevelid} 
                        placeholder="SuperLevelId"/>
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