using System;
using System.Collections;
using System.Collections.Generic;

namespace PokemonEssentials.Interface
{
	public interface IGameEditor
	{
		//UIntProperty			 	UIntProperty			{ get; }
		//LimitProperty			 	LimitProperty			{ get; }
		//NonzeroLimitProperty	 	NonzeroLimitProperty	{ get; }
		//ReadOnlyProperty		 	ReadOnlyProperty		{ get; }
		//EnumProperty			 	EnumProperty			{ get; }
		//LimitStringProperty	 		LimitStringProperty		{ get; }
		//UndefinedProperty		 	UndefinedProperty		{ get; }
		//BooleanProperty				BooleanProperty			{ get; }
		//StringProperty				StringProperty			{ get; }
		//BGMProperty			 		BGMProperty				{ get; }
		//MEProperty			 		MEProperty				{ get; }
		//WindowskinProperty	 		WindowskinProperty		{ get; }
		//TrainerTypeProperty	 		TrainerTypeProperty		{ get; }
		//SpeciesProperty		 		SpeciesProperty			{ get; }
		//TypeProperty			 	TypeProperty			{ get; }
		//MoveProperty			 	MoveProperty			{ get; }
		//ItemProperty			 	ItemProperty			{ get; }
		//NatureProperty				NatureProperty			{ get; }
	}
	public interface ILister : IDisposable
	{
		//ILister initialize(string folder, string selection);
		void setViewport(IViewport viewport);
		int startIndex { get; }
		IList<string> commands { get; }
		object value(int index);
		//void Dispose();
		void refresh(int index);
	}
	public interface IProperty
	{
		int set(string settingname, int? oldsetting);
		string format(string value);
	}
	public interface IProperty<TValue> : IProperty
	{
		//IProperty initialize(TValue maxdigits);
		int set(string settingname, TValue oldsetting);
		//string format(string value);
		//TValue defaultValue();
	}
	public interface IHasDefaultProperty<TValue> : IProperty<TValue>
	{
		//IProperty initialize(TValue maxdigits);
		//int set(string settingname, TValue oldsetting);
		//string format(string value);
		TValue defaultValue { get; }
	}
}