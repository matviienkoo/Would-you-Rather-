using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Michsky.MUIP
{
	public class PieChart : MaskableGraphic
	{
		// Chart Items
		[SerializeField] public List<PieChartDataNode> chartData = new List<PieChartDataNode>();

		// Settings
		[Range(-75, 150)] public float borderThickness = 5;
		[SerializeField] private Color borderColor = new Color32(255, 255, 255, 255);
		public Transform indicatorParent;
		public Animation valueSuffix;
		public bool addValueToIndicator = true;
		public bool enableBorderColor;

		private float fillAmount = 1f;
		private int segments = 720;

		// My animation
		private bool BoolRed;
		private float UpperProcent;

		private bool BoolBlue;
		private float LowerProcent;

		private int ProcentInt;

		[System.Serializable]
		public class PieChartDataNode
		{
			public string name = "Chart Item";
			public float value = 10;
			public Color32 color = new Color32(255, 255, 255, 255);
		}

		protected override void Awake()
		{
			base.Awake();
			UpdateIndicators();
		}

		private void Update()
		{
			this.borderThickness = (float)Mathf.Clamp(this.borderThickness, -75, rectTransform.rect.width / 3.333f);

			// Red chart animation
			if (BoolRed == true){
			this.enabled = false;
			this.enabled = true;

			if (ProcentInt == 1){
				chartData[0].value += 0.3f;
			}
			if (ProcentInt == 2){
				chartData[0].value -= 0.3f;
			}

				if (chartData[0].value >= UpperProcent)
				{
					chartData[0].value = UpperProcent;

					BoolRed = false;
				}
			}

			// Blue chart animation
			if (BoolBlue == true){
			this.enabled = false;
			this.enabled = true;

			if (ProcentInt == 1){
				chartData[1].value -= 0.3f;
			}
			if (ProcentInt == 2){
				chartData[1].value += 0.3f;
			}

				if (chartData[1].value <= LowerProcent)
				{
					chartData[1].value = LowerProcent;

					BoolBlue = false;
				}
			}
		}

		protected override void OnPopulateMesh(VertexHelper vh)
		{
			if (chartData.Count == 0)
				return;

			float outer = -rectTransform.pivot.x * rectTransform.rect.width;
			float inner = -rectTransform.pivot.x * rectTransform.rect.width + this.borderThickness;

			var outer1 = -rectTransform.pivot.x * rectTransform.rect.width * 0.6f;
			var inner1 = -rectTransform.pivot.x * rectTransform.rect.width * 0.6f + this.borderThickness;

			vh.Clear();

			Vector2 prevX = Vector2.zero;
			Vector2 prevY = Vector2.zero;
			Vector2 uv0 = new Vector2(0, 0);
			Vector2 uv1 = new Vector2(0, 1);
			Vector2 uv2 = new Vector2(1, 1);
			Vector2 uv3 = new Vector2(1, 0);
			Vector2 pos0;
			Vector2 pos1;
			Vector2 pos2;
			Vector2 pos3;

			float f = fillAmount;
			float degrees = 360f / segments;
			int fa = (int)((segments + 1) * f);

			var dataIndex = 0;
			var total = 0f;
			var currentValue = chartData[0].value;
			chartData.ForEach(s => total += s.value);
			var fillColor = chartData[0].color;

			for (int i = 0; i < fa; i++)
			{
				float rad = Mathf.Deg2Rad * (i * degrees);
				float c = Mathf.Cos(rad);
				float s = Mathf.Sin(rad);

				uv0 = new Vector2(0, 1);
				uv1 = new Vector2(1, 1);
				uv2 = new Vector2(1, 0);
				uv3 = new Vector2(0, 0);

				pos0 = prevX;
				pos1 = new Vector2(outer * c, outer * s);
				pos2 = new Vector2(inner * c, inner * s);
				pos3 = prevY;

				if (i > currentValue / total * segments)
				{
					if (dataIndex < chartData.Count - 1)
					{
						dataIndex += 1;
						currentValue += chartData[dataIndex].value;
						fillColor = chartData[dataIndex].color;
					}
				}

				vh.AddUIVertexQuad(SetVbo(new[] { pos0, pos1, pos2 * inner1 / inner, pos3 * inner1 / inner }, new[] { uv0, uv1, uv2, uv3 }, fillColor));

				if (enableBorderColor == true)
				{
					vh.AddUIVertexQuad(SetVbo(new[] { pos0, pos1, pos2, pos3 }, new[] { uv0, uv1, uv2, uv3 }, borderColor));
					vh.AddUIVertexQuad(SetVbo(new[] { pos0 * outer1 / outer, pos1 * outer1 / outer, pos2 * inner1 / inner, pos3 * inner1 / inner }, new[] { uv0, uv1, uv2, uv3 }, borderColor));
				}

				prevX = pos1;
				prevY = pos2;
			}
		}

		public void SetData(List<PieChartDataNode> data)
		{
			chartData = data;
			SetVerticesDirty();
		}

		protected UIVertex[] SetVbo(Vector2[] vertices, Vector2[] uvs, Color32 color)
		{
			UIVertex[] vbo = new UIVertex[4];

			for (int i = 0; i < vertices.Length; i++)
			{
				var vert = UIVertex.simpleVert;
				vert.color = color;
				vert.position = vertices[i];
				vert.uv0 = uvs[i];
				vbo[i] = vert;
			}

			return vbo;
		}

		public void UpdateIndicators()
		{
			for (int i = 0; i < chartData.Count; ++i)
			{

			}

			if (indicatorParent != null)
				StartCoroutine("UpdateIndicatorLayout");
		}

		public void AnimationPieChart ()
		{
			// Animation
			if (chartData[0].value >= UpperProcent || chartData[1].value >= LowerProcent)
			{
				ProcentInt = 1;
			}
			else
			ProcentInt = 2;

			BoolRed = true;
			BoolBlue = true;
		}

		public void DefualtPieChart ()
		{
			// Defualt numbers
			UpperProcent = 50;
			LowerProcent = 50;
			PlayerPrefs.SetFloat("UpperProcent", UpperProcent);
			PlayerPrefs.SetFloat("LowerProcent", LowerProcent);

			// Animation
			if (chartData[0].value >= UpperProcent || chartData[1].value >= LowerProcent)
			{
				ProcentInt = 1;
			}
			else
			ProcentInt = 2;

			BoolRed = true;
			BoolBlue = true;
		}

		public void AddNewItem()
		{
			PieChartDataNode item = new PieChartDataNode();

			if (indicatorParent.childCount != 0)
			{
				int tempIndex = indicatorParent.childCount - 1;
				
				GameObject tempIndicator = indicatorParent.GetChild(tempIndex).gameObject;
				GameObject newIndicator = Instantiate(tempIndicator, new Vector3(0, 0, 0), Quaternion.identity) as GameObject;
				
				newIndicator.transform.SetParent(indicatorParent, false);
				newIndicator.gameObject.name = "Item " + tempIndex.ToString() + " Indicator";
				
				item.name = "Chart Item " + tempIndex.ToString();
			}

			chartData.Add(item);
		}

		IEnumerator UpdateIndicatorLayout()
		{
			yield return new WaitForSeconds(0.1f);
			LayoutRebuilder.ForceRebuildLayoutImmediate(indicatorParent.GetComponentInParent<RectTransform>());
		}
	}
}