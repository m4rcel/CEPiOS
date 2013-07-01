using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
[AddComponentMenu("NGUI/NData/Texture Binding")]
public class NguiTextureBinding : NguiBinding
{
	private EZData.Property<Texture2D> _texture;
	private UITexture _uiTexture;
	
	private float width;
	private float height;
	
	public enum ALIGNMENT
	{
		STRETCH,
		UNIFORM_STRETCH,
	}
	
	public ALIGNMENT alignment = ALIGNMENT.UNIFORM_STRETCH;
	
	public override void Awake()
	{
		base.Awake();
		
		_uiTexture = gameObject.GetComponent<UITexture>();
		width = transform.localScale.x;
		height = transform.localScale.y;
	}
	
	public override void UpdateBinding()
	{
		if (_texture != null)
		{
			_texture.OnChange -= OnChange;
			_texture = null;
		}
		
		var context = GetContext();
		if (context == null)
		{
			Debug.LogWarning("EZTexture.UpdateBinding - context is null");
			return;
		}
		
		_texture = context.FindProperty<Texture2D>(Path, this);
		
		if (_texture != null)
		{
			_texture.OnChange += OnChange;
			OnChange();
		}
	}
	
	public void OnChange()
	{
		var aspect = (height == 0) ? 1 : (width / height);
		
		var imageWidth = width;
		var imageHeight = height;
		
		if (_texture != null && _texture.GetValue() != null)
		{
			imageWidth = _texture.GetValue().width;
			imageHeight = _texture.GetValue().height;
		}
		
		var imageAspect = (imageHeight == 0) ? 1 : (imageWidth / imageHeight);
		
		var spriteWidth = 0.0f;
		var spriteHeight = 0.0f;
		
		if (_texture != null && _texture.GetValue() != null)
		{
			switch(alignment)
			{
			case ALIGNMENT.STRETCH:
				spriteWidth = width;
				spriteHeight = height;
				break;
			case ALIGNMENT.UNIFORM_STRETCH:
				if (aspect > imageAspect)
				{
					spriteHeight = height;
					spriteWidth = (imageHeight == 0) ? 0 : (imageWidth * spriteHeight / imageHeight);
				}
				else
				{
					spriteWidth = width;
					spriteHeight = (imageWidth == 0) ? 0 : (imageHeight * spriteWidth / imageWidth);
				}
				break;
			}
		}
		
		if (_uiTexture != null)
		{
			if (_texture != null)
				_uiTexture.material.mainTexture = _texture.GetValue();
			_uiTexture.transform.localScale = new Vector3(spriteWidth, spriteHeight, 1);
		}
	}
}
