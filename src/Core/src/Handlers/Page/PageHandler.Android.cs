﻿using System;
using Android.Views;
using Microsoft.Maui.Graphics;

namespace Microsoft.Maui.Handlers
{
	public partial class PageHandler : ViewHandler<IPage, PageViewGroup>
	{
		//Graphics.Color? DefaultBackgroundColor;

		protected override PageViewGroup CreateNativeView()
		{
			if (VirtualView == null)
			{
				throw new InvalidOperationException($"{nameof(VirtualView)} must be set to create a PageViewGroup");
			}

			var viewGroup = new PageViewGroup(Context!)
			{
				CrossPlatformMeasure = VirtualView.Measure,
				CrossPlatformArrange = VirtualView.Arrange
			};

			return viewGroup;
		}

		protected override void SetupDefaults(PageViewGroup nativeView)
		{
			base.SetupDefaults(nativeView);
			//DefaultBackgroundColor = nativeView.bac
			var defaultBGColor = nativeView.Background;
		}

		public override void SetVirtualView(IView view)
		{
			base.SetVirtualView(view);
			_ = VirtualView ?? throw new InvalidOperationException($"{nameof(VirtualView)} should have been set by base class.");
			_ = NativeView ?? throw new InvalidOperationException($"{nameof(NativeView)} should have been set by base class.");

			NativeView.CrossPlatformMeasure = VirtualView.Measure;
			NativeView.CrossPlatformArrange = VirtualView.Arrange;
		}

		void UpdateContent()
		{
			_ = NativeView ?? throw new InvalidOperationException($"{nameof(NativeView)} should have been set by base class.");
			_ = MauiContext ?? throw new InvalidOperationException($"{nameof(MauiContext)} should have been set by base class.");
			_ = VirtualView ?? throw new InvalidOperationException($"{nameof(VirtualView)} should have been set by base class.");

			NativeView.RemoveAllViews();

			if (VirtualView.Content != null)
				NativeView.AddView(VirtualView.Content.ToNative(MauiContext));
		}

		public static void MapTitle(PageHandler handler, IPage page)
		{
		}

		public static void MapContent(PageHandler handler, IPage page)
		{
			handler.UpdateContent();
		}

		protected override void DisconnectHandler(PageViewGroup nativeView)
		{
			// If we're being disconnected from the xplat element, then we should no longer be managing its chidren
			NativeView?.RemoveAllViews();
			base.DisconnectHandler(nativeView);
		}
	}
}
