// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using Microsoft.AspNetCore.Mvc.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.Extensions.Localization;

namespace Hunter.WebUI.DataAnnotations.Internal
{
    public class MaxLengthAttributeAdapter : AttributeAdapterBase<MaxLengthAttribute>
    {
        private readonly string _max;

        public MaxLengthAttributeAdapter(MaxLengthAttribute attribute, IStringLocalizer stringLocalizer)
            : base(attribute, stringLocalizer)
        {
            _max = Attribute.Length.ToString(CultureInfo.InvariantCulture);
        }

        public override void AddValidation(ClientModelValidationContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            //MergeAttribute(context.Attributes, "data-val", "true");
            //MergeAttribute(context.Attributes, "data-val-maxlength", GetErrorMessage(context));
            //MergeAttribute(context.Attributes, "data-val-maxlength-max", _max);
            MergeAttribute(context.Attributes, "maxlength", _max);
            MergeAttribute(context.Attributes, "data-msg-maxlength", _max);
        }

        /// <inheritdoc />
        public override string GetErrorMessage(ModelValidationContextBase validationContext)
        {
            if (validationContext == null)
            {
                throw new ArgumentNullException(nameof(validationContext));
            }

            return GetErrorMessage(
                validationContext.ModelMetadata,
                validationContext.ModelMetadata.GetDisplayName(),
                Attribute.Length);
        }
    }
}
