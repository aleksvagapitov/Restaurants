import React from 'react'
import { FieldRenderProps } from 'react-final-form';
import { FormFieldProps, Form, Label, Dropdown } from 'semantic-ui-react';

interface IProps
  extends FieldRenderProps<string[], HTMLSelectElement>,
    FormFieldProps {}

const MultiSelectInput: React.FC<IProps> = ({
    input,
    width,
    options,
    placeholder,
    meta: { touched, error }
  }) => {
    return (
        <Form.Field error={touched && !!error} width={width}>
        <Dropdown 
            defaultValue={input.value}
            onChange={(e, data) => input.onChange(data.value)}
            fluid
            multiple
            search
            selection
            placeholder={placeholder}
            options={options}
        />
        {touched && error && (
          <Label basic color='red'>
            {error}
          </Label>
        )}
      </Form.Field>
    )
}

export default MultiSelectInput
